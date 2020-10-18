#!/bin/bash

pg_restore -d postgres --create -U $POSTGRES_USER --format=c /init.bac