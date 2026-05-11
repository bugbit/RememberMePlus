# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/).

## [1.0.4] - 2026-05-11
### Changed
- Updated README and AGENTS architecture documentation to reflect the current Clean Architecture directory structure and Dapper infrastructure separation.

## [1.0.3] - 2026-05-11
### Added
- Added application repository contract for reminder task persistence.
- Added separated Infrastructure/Data components for SQLite access with Dapper, connection creation, schema initialization, data models, mappers, and repository implementation.

## [1.0.2] - 2026-05-11
### Added
- Added initial domain model for reminder tasks with value objects and aggregate behavior.

## [1.0.1] - 2026-05-11
### Changed
- Reorganized MAUI presentation classes into a dedicated presentation layer structure.
- Replaced template counter code-behind behavior with MVVM binding through a home view model.
- Registered shell, page, and view model dependencies in the MAUI composition root.
