echo off
REM ZeroMQ Pineline pattern example
REM Author: Manar Ezzadeen
REM Blog  : http://idevhawk.phonezad.com
REM Email : idevhawk@gmail.com

cd /d %~dp0
start "Task Distributor (Push)" cmd /T:8F /k Push.exe -b tcp://127.0.0.1:5000 -m "Task #nb#" -x 5 -d 1000
start "Task Collector (Pull)" cmd /T:8E /k Pull.exe -b tcp://127.0.0.1:5001 -d 0
start "Worker 1" cmd /T:1F /k PullPushWorker.exe -l tcp://127.0.0.1:5000 -s tcp://127.0.0.1:5001 -t "#msg# (Worker 1)" -d 0
start "Worker 2" cmd /T:1F /k PullPushWorker.exe -l tcp://127.0.0.1:5000 -s tcp://127.0.0.1:5001 -t "#msg# (Worker 2)" -d 0
