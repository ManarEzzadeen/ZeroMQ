echo off
REM ZeroMQ Sync Pub-Sub pattern example 1
REM One Pub and two Sub (all messages subscription)
REM Author: Manar Ezzadeen
REM Blog  : http://idevhawk.phonezad.com
REM Email : idevhawk@gmail.com

cd /d %~dp0
start "Subscriber 1" cmd /T:4F /k SyncSub.exe -e tcp://127.0.0.1:5000 -d 0
start "Subscriber 2" cmd /T:4F /k SyncSub.exe -e tcp://127.0.0.1:5000 -d 4000
start "Publisher" cmd /T:0A /k SyncPub.exe -e tcp://127.0.0.1:5000 -p tcp://127.0.0.1:6000 -n 2 -m "Orange #nb#";"Apple  #nb#" -x 5 -d 1000