echo off
REM ZeroMQ Pub-Sub pattern example 3
REM Two Pub and one Sub
REM Author: Manar Ezzadeen
REM Blog  : http://idevhawk.phonezad.com
REM Email : idevhawk@gmail.com

cd /d %~dp0
start "Subscriber" cmd /T:8E /k Sub.exe -c tcp://127.0.0.1:5000;tcp://127.0.0.1:5001 -d 0
start "Orange Publisher" cmd /T:8F /k Pub.exe -b tcp://127.0.0.1:5000 -m "Orange #nb#" -x 5 -d 1000
start "Apple Publisher" cmd /T:8F /k Pub.exe -b tcp://127.0.0.1:5001 -m "Apple  #nb#" -x 5 -d 1000