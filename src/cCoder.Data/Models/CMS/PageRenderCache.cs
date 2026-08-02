// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

namespace cCoder.Data.Models.CMS;

public class PageRenderCache
{
    public int Id { get; set; }

    public int AppId { get; set; }

    public int PageId { get; set; }

    public string Culture { get; set; }

    public string Theme { get; set; }

    public string Value { get; set; }

    public string HeaderValue { get; set; }

    public string SourceFingerprint { get; set; }

    public DateTimeOffset RenderedOn { get; set; }
}