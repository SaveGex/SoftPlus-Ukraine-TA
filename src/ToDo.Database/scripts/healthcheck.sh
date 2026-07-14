#!bin/bash
set -e

/opt/mssql-tools18/bin/sqlcmd \
    -S localhost \
    -U "$MSSQL_USER" \
    -P "$MSSQL_USER_PASSWORD" \
    -Q "SELECT 1" \
    -C