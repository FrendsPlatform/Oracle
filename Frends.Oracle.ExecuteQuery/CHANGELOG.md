# Changelog

## [2.0.1] - 2022-11-04
### Fixed
- Fixed issue which resulted to task not able to close connection.

## [2.0.0] - 2022-10-31
### Fixed
- [Breaking] Simplified implementation and merged ConnectionProperties and QueryProperties classes into one Input class.
- Changed the result output object to be dynamic to enable dot notation.
- Changed the implementation to resemble other database related tasks.
- Modified the task to be asynchronous.
- Changed the task use different methods when doing 'select' query and when doing other queries.
- Updated workflow for Linux based testing and enabled the tests.

## [1.0.0] - 2022-04-22
### Added
- Initial implementation.
