# ADR-003: Use Unified RPC Endpoint for Business Operations

## Status

Accepted

## Context

CoreKit aims to provide highly standardized client-server communication.

Future modules should:

* require minimal API boilerplate
* have consistent request handling
* integrate naturally with CQRS

## Decision

Use unified business RPC endpoint:

POST /api/rpc

Business commands/queries are dispatched via MediatR.

Separate infrastructure endpoints may exist for:

* auth
* health
* system operations

## Consequences

### Positive

* Highly standardized API surface
* Minimal endpoint boilerplate
* Simplifies client abstraction
* Strong CQRS alignment

### Negative

* Less RESTful/HTTP-semantic
* Harder API discoverability without tooling

## Alternatives Considered

### Traditional REST Endpoints

Rejected because:

* too much repetitive boilerplate
* inconsistent API surface across modules

