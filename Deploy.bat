del publish\*.user /s
del publish\*.suo /s
del publish\*.sln /s
rd publish\.vs /s /q
rem xcopy publish\*.* /s \\Winserver\c\Development\Markus\Publish 