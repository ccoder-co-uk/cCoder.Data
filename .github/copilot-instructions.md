# The Standard: AI-Assisted Code Guidelines

You are assisting with code that follows **The Standard**, a unified engineering philosophy based on the Tri-Nature principle: **Purpose**, **Dependency**, and **Exposure**.

## Core Architecture (Tri-Nature)

Every system comprises three layers:

1. **Brokers (Dependencies)** - Wrappers around external resources, libraries, APIs, databases
2. **Services (Purpose)** - Business logic layer; the core differentiation between systems
3. **Exposers (Exposure)** - API controllers, UI endpoints, communication protocols

Each layer can be recursively subdivided into the same three components (fractal pattern).

## Mandatory Principles

### 1. Simplicity Over Complexity
- **Max one level of inheritance** (except version scaling)
- **No "Commons", "Utils", "Helpers"** - they create entanglement
- **No horizontal entanglement** - no shared models, exceptions, or validation rules across flows
- **No vertical entanglement** - no local base classes beyond native/external ones
- **Autonomous components** - each component owns its validations, utilities, and exceptions

### 2. What You See Is What You Get (WYSIWYG)
- All dependencies and logic must be explicit on the component
- No hidden routines, shared libraries, or magical extensions
- No runtime injection of unknown behavior
- Plain sight principle: everything is traceable and readable

### 3. Level 0 Engineering
- Code must be understandable by entry-level engineers
- Assume the reader has no context about the system
- Avoid clever tricks or obscure patterns
- Prioritize clarity over cleverness

### 4. Rewritability
- Every component should be easily rewritable
- No hidden prerequisites or undocumented dependencies
- Plug-and-play: fork, build, run tests with zero friction
- Business assumptions are always revisable

### 5. Airplane Mode (Cloud-Foreign)
- Systems must run fully locally without network dependencies
- Testable without cloud infrastructure
- Provide local tooling equivalents for cloud services (queues, event hubs, etc.)

### 6. Readability > Optimization
- When in doubt, choose readability
- Only optimize after profiling, never speculatively
- Comments explain "why", not "what"

### 7. Last Day Principle
- Every day's work must be in a complete, handoff-ready state
- Another engineer should be able to pick it up seamlessly
- Code should never be work-in-progress at end of day

## Layer Specifications

### Brokers (Dependencies)

**Purpose:** Model external dependencies that live outside your codebase (databases, APIs, queues, external libraries like Dapper, EF Core, etc.). Brokers are pure pass-throughs — they make external calls and return results, nothing more. They enable technology swapping by hiding the external contract behind a local interface.

**Ideal contract isolation:** In an ideal implementation, the broker fully absorbs the external contract so that the foundation service depends only on local models and never on any external type. If a broker wraps Dapper, for example, it should map its own internal/external types and expose only local contracts upward.

**Minimum acceptable:** When full isolation is not feasible, the mapping responsibility belongs to the **Foundation service**, not the broker. The foundation service maps local models to whatever the broker/external dependency requires, and maps external responses back to local models. The broker itself still performs no mapping logic — it only passes data to and from the external dependency as-is.

**Rules:**
- ✅ Pure pass-through to external dependency — one call in, one result out
- ✅ Implement local interface contracts only (hide external contracts from upper layers)
- ✅ Own their configuration and connection logic
- ✅ Speak the language of their technology (SQL: "Select", Queue: "Enqueue", API: "Post/Get/Put")
- ✅ One resource = one broker (one-to-one relationship)
- ❌ No business logic of any kind
- ❌ No flow control (if/while/switch)
- ❌ No exception handling (let exceptions propagate)
- ❌ No mapping or transformation of data (mapping belongs in the Foundation service)
- ❌ No other brokers as dependencies
- ❌ No services as dependencies

**Structure:** Use partial classes for brokers supporting multiple entities

### Services (Business Logic)

**Three operation categories:**
1. **Validations** - Structural, logical, external (in that order)
2. **Processing** - Flow control, mapping, computation
3. **Integration** - Retrieve/push data via brokers

**Service Types:**

**Foundations (Validator)** - Broker-neighboring, add validation layer to CRUD

**Orchestrators** - Combine multiple operations; decision-makers; flow controllers; 2-3 service dependencies (Florance Pattern: not 1, not 4+). Also called Coordinators when orchestrating other orchestrations (orchestration of orchestration).

**Aggregators** - Gatekeepers; expose single contact point for exposers; coordinate multiple operations

**Service Rules:**
- ✅ Do or delegate work (pick one)
- ✅ Orchestrators have 2-3 service dependencies
- ✅ Orchestrators must depend on at least 2 services; never 1 (Florance Pattern)
- ✅ Orchestrators must include service dependencies (typically Processing), not brokers-only
- ✅ Own all validations, exceptions, error mapping
- ✅ Technology-agnostic
- ✅ Dependencies must be cross-level only (downward), never same-level
- ❌ Orchestrators cannot call/depend on other Orchestrators
- ❌ No same-level service dependencies (Aggregation->Aggregation, Coordination->Coordination, Orchestration->Orchestration, Processing->Processing, Foundation->Foundation)
- ❌ No shared validation rules across flows
- ❌ No "magic" shared code

**Aggregator-Specific Rules:**
- ✅ Pure pass-through orchestration only
- ✅ No business logic whatsoever
- ✅ No computation, validation, or processing
- ✅ Simply coordinate and combine results from lower services
- ❌ No conditional logic (if/else based on data)
- ❌ No data transformation or mapping logic
- ❌ No exception handling beyond propagation

**Processing & Foundation Service Rules:**
- ✅ Single entity/contract per service (e.g., StudentProcessingService handles Student only)
- ✅ Void and Task returns are exempt from single-entity rule (fire-and-forget operations)
- ✅ Multiple methods allowed if they all operate on the same entity
- ❌ No mixing multiple unrelated entities in one service

**Coordinator & Below Single-Contract Rule:**
- ✅ Coordinators, Orchestrators, Processing, and Foundations each handle ONE entity/contract only
- ✅ Service naming reflects entity: StudentCoordinator, InvoiceOrchestrationService, StudentFoundationService
- ✅ All public methods must operate on the same entity type
- ❌ Mixing different entities in one service (e.g., StudentOrchestrationService cannot handle Invoice)

**Service Call Hierarchy (Strictly Downward Only):**

Each service level can ONLY call the immediate level below + utility brokers. No skipping levels.

Same-level dependencies are prohibited across the board, including brokers.

| Service Level | Can Call | Examples |
|---|---|---|
| **Exposers** | Aggregators, Coordinators | API controllers are the top entry point |
| **Aggregators** | Coordinators only | Coordinate multiple flows; expose single API |
| **Coordinators** | Orchestrators, Utility Brokers | Orchestration of orchestrations |
| **Orchestrators** | Processing, Utility Brokers | Decision-makers; flow control (Florance: 2-3 deps, never Orchestrator-to-Orchestrator) |
| **Processing** | Foundations, Utility Brokers | Business logic execution; no orchestration |
| **Foundations** | Brokers only | Validation + CRUD wrapper |
| **Brokers** | External resources only | No service dependencies |

**Golden Rule:** Strict downward flow enforces Florance Pattern. Each level calls exactly one level below (+ utility brokers). No level-skipping. No upward dependencies.

**Lateral Rule:** No same-level dependencies: Aggregation->Aggregation, Coordination->Coordination, Orchestration->Orchestration, Processing->Processing, Foundation->Foundation, Broker->Broker.

**Examples:**

```
StudentAggregationService (PASS-THROUGH ONLY):
  ✅ public async Task<StudentAggregation> GetStudentWithCoursesAsync(Guid id)
       {
           var student = await this.coordinatorService.GetStudentAsync(id);
           var courses = await this.coordinatorService.GetStudentCoursesAsync(id);
           return new StudentAggregation { Student = student, Courses = courses };
       }
  ❌ NO if/else logic, NO data transformation, NO business decisions

InvoiceOrchestrationService can ONLY call:
  ✅ InvoiceProcessingService (immediate level below)
  ✅ Utility Brokers (StorageBroker, NotificationBroker, ConfigurationBroker, etc.)
  ✅ At least one additional service dependency (total service deps must be 2-3)
  ❌ InvoiceFoundationService (skips level = violation)
  ❌ AggregatorServices (upward = violation)
  ❌ Other OrchestrationServices (orchestration-to-orchestration is not allowed)
  ❌ A single dependency only (if only one dependency, this is not orchestration)
  ❌ Brokers-only dependency graph (must include service dependencies)
  ❌ StorageService that is orchestration or processing level

InvoiceProcessingService can ONLY call:
  ✅ InvoiceFoundationService (immediate level below)
  ✅ Utility Brokers
  ❌ InvoiceOrchestrationService (upward = violation)
  ❌ Other ProcessingServices directly (only via Orchestrator)

Broker rules also imply:
  ❌ Broker->Broker dependencies are not allowed
```

### Exposers (Exposure)

**Purpose:** Present business logic to outside world

**Types:** API controllers, UI endpoints, communication protocols

**Rules:**
- ✅ Can call any service level (Aggregators, Orchestrators, Processing, Foundations)
- ✅ Prefer single contact point (Aggregators) for cohesion, but not required
- ✅ Technology-specific naming/conventions allowed
- ❌ No business logic
- ❌ No direct broker calls

## Anti-Patterns (Prohibited)

1. **Excessive Inheritance** - More than one level (breaks simplicity)
2. **Shared Components** - Utils, Helpers, Commons (create entanglement)
3. **Shared Models** - Exceptions, validation rules across flows (hidden coupling)
4. **Local Base Classes** - Create vertical entanglement (harm rewritability)
5. **Flow Control in Brokers** - Belongs in services
6. **Exception Handling in Brokers** - Exceptions propagate
7. **Cloud-Native Dependencies** - System must work offline
8. **Hidden Magic** - Runtime behavior injection, unexplained dependencies
9. **Optimization Over Readability** - Readable code is truly optimized code

## Non-Negotiables

- **All-In/All-Out** - Standard is fully embraced or rejected; partial adoption isn't valid
- **No Toasters** - Compliance driven by conviction, not automated linters/analyzers (though automation supports human judgment)
- **Human Authority** - AI assists; humans decide and approve
- **Pass Forward** - Share knowledge freely at no cost
- **Airplane Mode** - No hard dependency on cloud/network
- **Continuity** - Daily work handoff-ready

## Implementation Checklist

When reviewing/writing code:

- [ ] Can each component's full logic be understood in one read?
- [ ] Are all dependencies explicit (no hidden injections)?
- [ ] Does each component own its validations and exceptions?
- [ ] Is there code duplication? (Good if autonomy favors it)
- [ ] Can this be rewritten in one day by a new engineer?
- [ ] Are there any "Commons" or "Utils" folders?
- [ ] Do services have 2-3 dependencies (if orchestrators)?
- [ ] Do brokers have flow control or exception handling?
- [ ] Can this run without network/cloud?
- [ ] Is this code Level 0 (entry-engineer readable)?

## Guidance for AI

When assisting:

1. **Suggest autonomous components** over shared code
2. **Favor code duplication** if it preserves clarity and independence
3. **Question entanglement** - is this shared code necessary or harmful?
4. **Recommend breaking up** services with 4+ dependencies
5. **Challenge inheritance** - is there a simpler way?
6. **Suggest interfaces** for external dependencies (broker contracts)
7. **Push for WYSIWYG** - make everything visible and traceable
8. **Verify Level 0** - can a junior understand this?
9. **Check rewritability** - could this be rewritten in a day?
10. **Highlight coupling** - is there hidden dependency or magic?

---

**Core Belief:** *Simplicity, clarity, and independence enable systems that survive, evolve, and endure.*
