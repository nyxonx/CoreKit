# ADR-005: Use Blazor WebAssembly Hosted PWA

## Status

Accepted

## Context

CoreKit client must:

* maximize client resource usage
* support desktop and mobile browser usage
* be installable as app
* provide future offline/PWA foundation

## Decision

Use Blazor WebAssembly Hosted with PWA enabled.

## Consequences

### Positive

* Rich client-side execution
* Better client resource utilization
* Shared .NET stack across client/server
* PWA installability
* Good future mobile/web flexibility

### Negative

* Larger initial payload than server-side Blazor
* More client-side complexity

## Alternatives Considered

### Blazor Server

Rejected because:

* server resource dependence
* persistent connection requirement
* weaker offline/mobile/PWA story

### Separate SPA Framework

Rejected because:

* split tech stack
* reduced code sharing

