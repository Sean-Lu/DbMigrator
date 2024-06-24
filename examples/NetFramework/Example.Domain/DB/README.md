## 数据库迁移升级说明

- 支持多数据库独立升级：MigrationService.Upgrade()
- 支持2种升级方式：执行SQL脚本（DB\Scripts）、执行代码（DB\Migrations）
- 自动执行SQL脚本文件命名规则：V{版本号}__{描述}.sql
- 版本号规则（Migration version）：yyyyMMddHHmmss
