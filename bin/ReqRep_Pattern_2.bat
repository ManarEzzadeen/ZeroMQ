echo off
REM ZeroMQ Req-Rep pattern example 2
REM Two Reb and one Req
REM Author: Manar Ezzadeen
REM Blog  : http://idevhawk.phonezad.com
REM Email : idevhawk@gmail.com

cd /d %~dp0
start "Server 1 (Rep)" cmd /T:8E /k Rep.exe -b tcp://127.0.0.1:5000 -r "#msg# Reply 1" -d 0
start "Server 2 (Rep)" cmd /T:8E /k Rep.exe -b tcp://127.0.0.1:5001 -r "#msg# Reply 2" -d 0
start "Client (Req)" cmd /T:8F /k Req.exe -c tcp://127.0.0.1:5000;tcp://127.0.0.1:5001 -m "Request  #nb#" -x 5 -d 1000
