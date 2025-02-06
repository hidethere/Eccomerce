#!/bin/bash

echo "Deploying MSSQL Data Storage"
kubectl apply -f mssql/mssql-pvc.yaml
kubectl apply -f mssql/mssql-statefulset.yaml
kubectl get pods -l app=mssql

echo "Deploying User service"
kubectl apply -f user-service/user-service.yaml
kubectl get pods -l app=user-service


echo "Deployment Completed!"
