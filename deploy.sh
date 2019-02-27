mkdir -p Assemblies
mv TwitchStories.* ./Assemblies

languages="Catalan ChineseSimplified ChineseTraditional Czech Danish Dutch Estonian Finnish French Hungarian Italian Japanese Korean Norwegian Polish Portuguese PortugueseBrazilian Romanian Slovak Spanish SpanishLatin Swedish Turkish Ukrainian"
for language in $languages
do
  mkdir -p ./Languages/$language
  cp -r -n ./Languages/English/* ./Languages/$language
done

mkdir -p ~/.steam/steam/steamapps/common/RimWorld/Mods/TwitchStories
cp -r -f * ~/.steam/steam/steamapps/common/RimWorld/Mods/TwitchStories/
rm ~/.steam/steam/steamapps/common/RimWorld/Mods/TwitchStories/deploy.sh