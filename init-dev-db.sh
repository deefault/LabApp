#!/bin/bash
echo "Begin develop database initialization from backup"
pg_restore -d postgres --create -U $POSTGRES_USER --format=c /init.bac