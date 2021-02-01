#!/bin/bash

set -e
run_cmd="dotnet run --server.urls http://*:5000"

mkdir -p videos

>&2 echo "mySQL is up"
exec $run_cmd


