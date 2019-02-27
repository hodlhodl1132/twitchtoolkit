mkdir Assemblies
move /Y TwitchStories.* Assemblies

set languages=Catalan ChineseSimplified ChineseTraditional Czech Danish Dutch Estonian Finnish French Hungarian Italian Japanese Korean Norwegian Polish Portuguese PortugueseBrazilian Romanian Slovak Spanish SpanishLatin Swedish Turkish Ukrainian
for %%l in (%languages%) do (
  echo %%l
  mkdir Languages\%%l
  %systemroot%\System32\xcopy /E /Y Languages\English\* Languages\%%l\
)


mkdir "C:\Program Files (x86)\Steam\steamapps\common\RimWorld\Mods"
%systemroot%\System32\xcopy /E /Y * "C:\Program Files (x86)\Steam\steamapps\common\RimWorld\Mods\TwitchStories\"
del "C:\Program Files (x86)\Steam\steamapps\common\RimWorld\Mods\TwitchStories\deploy.bat"