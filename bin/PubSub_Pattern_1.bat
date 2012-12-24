echo off
REM ZeroMQ Pub-Sub pattern example 1
REM One Pub and two Sub (all messages subscription)
REM Author: Manar Ezzadeen
REM Blog  : http://idevhawk.phonezad.com
REM Email : idevhawk@gmail.com

cd /d %~dp0
start "Subscriber 1" cmd /T:8E /k Sub.exe -c tcp://127.0.0.1:5000 -d 0
start "Subscriber 2" cmd /T:8E /k Sub.exe -c tcp://127.0.0.1:5000 -d 0
start "Publisher" cmd /T:8F /k Pub.exe -b tcp://127.0.0.1:5000 -m "Orange #nb#";"Apple  #nb#" -x 5 -d 1000