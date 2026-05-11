# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to semantic versioning.

## [Unreleased]

## [1.1.1] - 2026-05-11
### Changed
- Updated `README.md` with the current domain and infrastructure directory structure.

## [1.1.0] - 2026-05-11
### Added
- Added the reminder task domain model under `Domain/Tasks` with value objects, priority classification, aggregate behavior, and repository abstraction.
- Added SQLite/Dapper infrastructure folders, options, connection factory, schema initializer, mapper, data model, and repository implementation.
- Added infrastructure dependency injection registration for local SQLite persistence services.

### Changed
- Updated the MAUI project version from `1.0`/`1` to `1.1.0`/`2`.
