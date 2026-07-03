(function () {
    const state = {
        entitySets: [],
        currentEntitySet: null,
        rows: [],
        editingRow: null,
        editingMode: "create"
    };

    const elements = {
        body: document.body,
        loginForm: document.getElementById("loginForm"),
        authUser: document.getElementById("authUser"),
        usernameInput: document.getElementById("usernameInput"),
        passwordInput: document.getElementById("passwordInput"),
        logoutButton: document.getElementById("logoutButton"),
        statusText: document.getElementById("statusText"),
        entityTabs: document.getElementById("entityTabs"),
        entityTitle: document.getElementById("entityTitle"),
        entityDescription: document.getElementById("entityDescription"),
        createRowButton: document.getElementById("createRowButton"),
        dataGrid: document.getElementById("dataGrid"),
        editorDialog: document.getElementById("editorDialog"),
        editorTitle: document.getElementById("editorTitle"),
        editorFields: document.getElementById("editorFields"),
        saveDialogButton: document.getElementById("saveDialogButton"),
        closeDialogButton: document.getElementById("closeDialogButton")
    };

    function setStatus(message, isError) {
        elements.statusText.textContent = message;
        elements.statusText.classList.toggle("data-status-error", !!isError);
    }

    function setAuthenticated(isAuthenticated) {
        elements.body.classList.toggle("is-authenticated", isAuthenticated);
        elements.authUser.textContent = dataApi.getUser();
    }

    async function initialise() {
        setAuthenticated(!!dataApi.getToken());
        bindEvents();

        if (dataApi.getToken()) {
            await loadEntitySets();
        }
    }

    function bindEvents() {
        elements.loginForm.addEventListener("submit", async event => {
            event.preventDefault();
            await run(async () => {
                await dataApi.login(
                    elements.usernameInput.value,
                    elements.passwordInput.value);

                elements.passwordInput.value = "";
                setAuthenticated(true);
                await loadEntitySets();
            });
        });

        elements.logoutButton.addEventListener("click", () => {
            dataApi.setToken(null);
            state.entitySets = [];
            state.currentEntitySet = null;
            state.rows = [];
            elements.entityTabs.replaceChildren();
            elements.dataGrid.replaceChildren();
            setAuthenticated(false);
            setStatus("Ready");
        });

        elements.createRowButton.addEventListener("click", () => {
            openEditor("create");
        });

        elements.saveDialogButton.addEventListener("click", async () => {
            await saveEditor();
        });

        elements.closeDialogButton.addEventListener("click", () => {
            elements.editorDialog.close();
        });
    }

    async function loadEntitySets() {
        await run(async () => {
            state.entitySets = await dataApi.request("/Api/Data/EntitySets");
            renderTabs();

            if (state.entitySets.length) {
                await selectEntitySet(state.entitySets[0].name);
            }
        });
    }

    function renderTabs() {
        elements.entityTabs.replaceChildren();

        for (const entitySet of state.entitySets) {
            const button = document.createElement("button");
            button.type = "button";
            button.textContent = entitySet.displayName;
            button.className = entitySet === state.currentEntitySet ? "active" : "";
            button.addEventListener("click", () => selectEntitySet(entitySet.name));
            elements.entityTabs.appendChild(button);
        }
    }

    async function selectEntitySet(name) {
        state.currentEntitySet = state.entitySets.find(entitySet => entitySet.name === name);
        renderTabs();

        elements.entityTitle.textContent = state.currentEntitySet.displayName;
        elements.entityDescription.textContent = `${state.currentEntitySet.table} rows from ${state.currentEntitySet.clrType}`;
        elements.createRowButton.textContent = `Create ${state.currentEntitySet.displayName}`;

        await loadRows();
    }

    async function loadRows() {
        await run(async () => {
            const result = await dataApi.request(`/Api/Data/${state.currentEntitySet.name}?take=100`);
            state.rows = result.rows || [];
            renderGrid();
        });
    }

    function renderGrid() {
        elements.dataGrid.replaceChildren();

        const table = document.createElement("table");
        table.className = "data-table";
        table.appendChild(renderHead());
        table.appendChild(renderBody());
        elements.dataGrid.appendChild(table);
    }

    function renderHead() {
        const thead = document.createElement("thead");
        const row = document.createElement("tr");

        for (const property of state.currentEntitySet.properties) {
            const th = document.createElement("th");
            th.textContent = property.name;
            row.appendChild(th);
        }

        const actions = document.createElement("th");
        actions.textContent = "Actions";
        actions.className = "data-actions";
        row.appendChild(actions);
        thead.appendChild(row);
        return thead;
    }

    function renderBody() {
        const tbody = document.createElement("tbody");

        if (!state.rows.length) {
            const row = document.createElement("tr");
            const cell = document.createElement("td");
            cell.colSpan = state.currentEntitySet.properties.length + 1;
            cell.className = "data-empty";
            cell.textContent = `No ${state.currentEntitySet.displayName} rows found.`;
            row.appendChild(cell);
            tbody.appendChild(row);
            return tbody;
        }

        for (const dataRow of state.rows) {
            const row = document.createElement("tr");

            for (const property of state.currentEntitySet.properties) {
                const cell = document.createElement("td");
                appendValue(cell, dataRow[property.name]);
                row.appendChild(cell);
            }

            const actions = document.createElement("td");
            actions.className = "data-actions";
            actions.appendChild(createActionButton("Edit", () => openEditor("edit", dataRow)));
            actions.appendChild(createActionButton("Delete", () => deleteRow(dataRow)));
            row.appendChild(actions);
            tbody.appendChild(row);
        }

        return tbody;
    }

    function appendValue(cell, value) {
        if (typeof value === "object" && value !== null) {
            const pre = document.createElement("pre");
            pre.textContent = JSON.stringify(value, null, 2);
            cell.appendChild(pre);
            return;
        }

        const text = value === null || value === undefined
            ? ""
            : String(value);

        if (text.length > 120 || text.startsWith("{") || text.startsWith("[")) {
            const pre = document.createElement("pre");
            pre.textContent = text;
            cell.appendChild(pre);
            return;
        }

        cell.textContent = text;
    }

    function createActionButton(text, action) {
        const button = document.createElement("button");
        button.type = "button";
        button.textContent = text;
        button.addEventListener("click", action);
        return button;
    }

    function openEditor(mode, row) {
        state.editingMode = mode;
        state.editingRow = row || {};
        elements.editorTitle.textContent = mode === "create"
            ? `Create ${state.currentEntitySet.displayName}`
            : `Edit ${state.currentEntitySet.displayName}`;

        renderEditorFields(mode);
        elements.editorDialog.showModal();
    }

    function renderEditorFields(mode) {
        elements.editorFields.replaceChildren();

        for (const property of state.currentEntitySet.properties) {
            const canWrite = mode === "create"
                ? property.canCreate
                : property.canUpdate || property.isKey;

            if (!canWrite) {
                continue;
            }

            const label = document.createElement("label");
            const caption = document.createElement("span");
            caption.textContent = property.name;
            label.appendChild(caption);

            const input = createInput(property, state.editingRow[property.name], mode);
            label.appendChild(input);
            elements.editorFields.appendChild(label);
        }
    }

    function createInput(property, value, mode) {
        const input = property.isLongText
            ? document.createElement("textarea")
            : document.createElement("input");

        input.name = property.name;
        input.dataset.type = property.type;
        input.dataset.nullable = property.isNullable ? "true" : "false";
        input.disabled = mode === "edit" && property.isKey;

        if (property.type === "Boolean") {
            input.type = "checkbox";
            input.checked = value === true || value === "true";
        } else {
            input.type = "text";
            input.value = value === null || value === undefined ? "" : String(value);
        }

        return input;
    }

    async function saveEditor() {
        await run(async () => {
            const payload = readEditorPayload();
            const method = state.editingMode === "create" ? "POST" : "PUT";

            await dataApi.request(`/Api/Data/${state.currentEntitySet.name}`, {
                method,
                body: JSON.stringify(payload)
            });

            elements.editorDialog.close();
            await loadRows();
        });
    }

    function readEditorPayload() {
        const payload = {};

        for (const input of elements.editorFields.querySelectorAll("input, textarea")) {
            if (input.disabled && !isKey(input.name)) {
                continue;
            }

            payload[input.name] = readInputValue(input);
        }

        if (state.editingMode === "edit") {
            for (const key of state.currentEntitySet.keyProperties) {
                payload[key] = state.editingRow[key];
            }
        }

        return payload;
    }

    function readInputValue(input) {
        if (input.type === "checkbox") {
            return input.checked;
        }

        if (input.value === "" && input.dataset.nullable === "true") {
            return null;
        }

        if (input.dataset.type === "Int32"
            || input.dataset.type === "Int64"
            || input.dataset.type === "Int16"
            || input.dataset.type === "Decimal"
            || input.dataset.type === "Double"
            || input.dataset.type === "Single") {
            return Number(input.value);
        }

        return input.value;
    }

    function isKey(propertyName) {
        return state.currentEntitySet.keyProperties.some(key => key === propertyName);
    }

    async function deleteRow(row) {
        if (!confirm(`Delete this ${state.currentEntitySet.displayName} row?`)) {
            return;
        }

        await run(async () => {
            const payload = {};

            for (const key of state.currentEntitySet.keyProperties) {
                payload[key] = row[key];
            }

            await dataApi.request(`/Api/Data/${state.currentEntitySet.name}`, {
                method: "DELETE",
                body: JSON.stringify(payload)
            });

            await loadRows();
        });
    }

    async function run(action) {
        try {
            setStatus("Working...");
            await action();
            setStatus("Ready");
        } catch (error) {
            setStatus(error.message || "Something went wrong", true);
        }
    }

    initialise();
})();
