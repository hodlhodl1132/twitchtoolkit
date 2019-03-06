# Build
Either use your preferred way to compile C# code, just make sure to target .NET 3.5 and then manually copy the files to the RimWorld Mods folder.
Or you can use the provided Makefile, which also automates the copying part including filling up missing languages.

# Contribute
Just fork this repository, add your changes, push them to your fork and open a merge request to merge your changes into this repository.
Also feel free to join the [Discord](https://discord.gg/qrtg224) if you have any questions or want to hang out.

# Translations
We are currently not accepting translations, I am sorry for the inconvenience.

# Commands
## User Commands:

**!balance, !bal, !coins** - check balance and karma rate

**!buyevent** - Syntax: !buyevent skillincrease - purchase an event

**!buyitem** - Syntax: !buyitem beer 2 - purchase an item

**!whatiskarma, !karma** - explains what the karma system is

**!purchaselist, !instructions** - gives users a link to the public purchase list and info on using the mod

**!modinfo** - gives users info about the mod

## Admin Commands:

**!refreshviewers** - updates viewers watching channel, this is for debugging mostly

**!karmaround** - simulates a coin reward round

**!givecoins** - Syntax: !givecoins @username 1000 - gives user 1000 coins

**!giveallcoins** - Syntax: !giveallcoins 1000 - gives all users 1000 coins (can also be used with negative numbers to take coins)

**!resetviewers** - resets all viewers data back to default, must confirm twice

**!checkuser** - Syntax: !checkuser @username - do a balance check on the user

**!setkarma** - Syntax: !setuserkarma @username 100 - set user karma to 100% (needs message)

**!togglestore** - toggle ability for users to purchase items/events

**!togglecoins** - toggle ability for users to earn coins while viewing
