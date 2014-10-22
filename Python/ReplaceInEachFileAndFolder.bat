REM SET Below to Python Path on your machine

REM Command Format: python {PYTHONFILE}.py [Params] > [log file]

SET PYTHON_PATH="C:\Python27\ArcGIS10.1"

%PYTHON_PATH%\python ReplaceInEachFileAndFolder.py . > python_log.txt

echo Done!
pause
