echo off
REM ZeroMQ Req-Rep pattern example 1
REM One Req and one Reb
REM Author: Manar Ezzadeen
REM Blog  : http://idevhawk.phonezad.com
REM Email : idevhawk@gmail.com

cd /d %~dp0
start "Server (Rep)" cmd /T:8E /k Rep.exe -b tcp://127.0.0.1:5000 -r "#msg# - Reply" -d 0
start "Client (Req)" cmd /T:8F /k Req.exe -c tcp://127.0.0.1:5000 -m "Request  #nb#" -x 5 -d 1000