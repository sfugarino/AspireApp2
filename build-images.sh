#!/usr/bin/env bash

cd /home/sam/dev/AspireApp2

docker rmi aspireapp2apiservice:arm64v8 -f
docker rmi aspireapp2web:arm64v8 -f

docker rmi sfugarino/aspireapp2apiservice:arm64v8 -f
docker rmi sfugarino/aspireapp2web:arm64v8 -f

docker buildx build --platform linux/arm64 -f AspireApp2.ApiService/Dockerfile -t aspireapp2apiservice:arm64v8 .
docker image tag aspireapp2apiservice:arm64v8 sfugarino/aspireapp2apiservice:arm64v8

docker buildx build --platform linux/arm64 -f AspireApp2.Web/Dockerfile -t aspireapp2web:arm64v8 .
docker image tag aspireapp2web:arm64v8 sfugarino/aspireapp2web:arm64v8

docker push sfugarino/aspireapp2apiservice:arm64v8
docker push sfugarino/aspireapp2web:arm64v8

