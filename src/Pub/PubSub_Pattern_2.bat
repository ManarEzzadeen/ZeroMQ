echo off
REM ZeroMQ Pub-Sub pattern example 2
REM One Pub and two Sub (specific message subscription)
REM Author: Manar Ezzadeen
REM Blog  : http://idevhawk.phonezad.com
REM Email : idevhawk@gmail.com

cd /d %~dp0
start "Subscriber 1" cmd /T:8E /k Sub.exe -c tcp://127.0.0.1:5000 -s "Orange";"Apple" -d 0
start "Subscriber 2" cmd /T:8E /k Sub.exe -c tcp://127.0.0.1:5000 -s "Kiwi" -d 0
start "Publisher" cmd /T:8F /k Pub.exe -b tcp://127.0.0.1:5000 -m "Orange #nb#";"Apple  #nb#";"Kiwi   #nb#" -x 7 -d 1000