@echo off
@for /f "delims=" %%a in ('dir /b/a-d/on *.ts') do tsc %%a --sourcemap
ping -n 30 127.0.0.1>nul 
