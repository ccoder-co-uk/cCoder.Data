// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

namespace cCoder.Data.Models.CMS;

public class PageRenderCache
{
    public string Id { get; set; }

    public int AppId { get; set; }

    public int PageId { get; set; }

    public string Culture { get; set; }

    public string Theme { get; set; }

    public int? ParentId { get; set; }

    public string Path { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public string Keywords { get; set; }

    public bool ShowOnMenus { get; set; }

    public string Header { get; set; }

    public string Body { get; set; }

    public string SourceFingerprint { get; set; }

    public DateTimeOffset RenderedOn { get; set; }
}