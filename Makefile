ifeq ($(OS),Windows_NT)
	MSBUILD=C:/Windows/Microsoft.NET/Framework/v4.0.30319/msbuild.exe
	ifeq ($(DEBUG),1)
		COMPILE=$(MSBUILD) -p:Configuration="Debug"
		OUTPUTPATH=TwitchStories/bin/Debug/
	else
		COMPILE=$(MSBUILD) -p:Configuration="Release"
		OUTPUTPATH=TwitchStories/bin/Release/
	endif
	SCRIPT=deploy.bat
	COPYSCRIPT=cp $(SCRIPT) $(OUTPUTPATH)
	RUNSCRIPT=cmd /c "cd $(OUTPUTPATH) & $(SCRIPT) & del $(SCRIPT)"
else
	ifeq ($(DEBUG),1)
		COMPILE=xbuild /p:Configuration="Debug"
		OUTPUTPATH=TwitchStories/bin/Debug/
	else
		COMPILE=xbuild /p:Configuration="Release"
		OUTPUTPATH=TwitchStories/bin/Release/
	endif
	SCRIPT=deploy.sh
	COPYSCRIPT=cp $(SCRIPT) $(OUTPUTPATH)
	RUNSCRIPT=cd $(OUTPUTPATH); sh $(SCRIPT); rm -rf $(SCRIPT)
endif

all:
	$(COMPILE)
	$(COPYSCRIPT)
	$(RUNSCRIPT)