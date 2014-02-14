REM TODO: Change these paths to match your configuration

SET comparer=C:\DefenseTemplates\Github\MyMiscellanea\Qt\build-CompareImages-Desktop-Release\release\CompareImages.exe
SET source_images_folder=C:\DefenseTemplates\Github\MyMiscellanea\Qt\CompareImages\CompareImagesData\Case1
SET compare_images_folder=C:\DefenseTemplates\Github\MyMiscellanea\Qt\CompareImages\CompareImagesData\Case2

for /r "%source_images_folder%" %%i in (*.png) do "%comparer%" "%%i" "%compare_images_folder%\%%~ni.png"
