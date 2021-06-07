# Smee
A tool which helps with setting up git hooks

### Git
Git provides a wonderful hooks feature which allows you to write simple shell scripts that get executed during various events. These scripts can transform your code, 
block commits that dont pass muster, or really whatever you want. However the functionality has some serious pain points:

* Prior to git 2.9, The scripts themselves live in .git\hooks which makes them specific to each local repository on the developer's machine
* After git 2.9 the scripts directory can be configured but this is extra weird on windows
* Using this feature on windows oddly difficult on windows due to how git for windows executes them. Most people who do use them end up writing wrapper shell scripts that invoke powershell

### Husky

Many developers use an NPM package, Husky, to help aleviate these pain points. However Husky has its own issues

* Older versions of husky depend on the NPM lifecycle events which is a real drag if you're not using node in your project
* The most recent versions of husky have changed and no longer alleviate the pain points on windows

### Smee

Smee attempts to be as independent of platforms or tooling as possible. It will help you setup your git hooks in whatever language you want while
dealing with the quirks of the underlying platform and git version.

## Installation
Currently I havent put together any packages for this tool. Compile it and make sure the smee executable is in your path with execute permissions.

## Usage

### TLDR;
To clone a git repo with hooks already setup
```
git clone <your repo>
cd <your repo>
smee init
```
To start a new repo from scratch
```
mkdir <your repo> 
cd  <your repo> 
smee init 
smee add pre-commit (commit-msg ... )
```

to start a new repo with common hooks as a submodule
```
mkdir <your repo> 
cd  <your repo> 
smee init --from-repo <your hooks repo>
```


### Common parameters
All smee commands take two optional parameters. `-r|--repo-path` the absolute path of a git repository. `-h|--hooks-path`, a directory relative to the root of the git repo where the hooks will be stored.


### Initializing git hooks

Once you've created a new git repo and are ready to start writing hooks scripts you'll need to run the init command
```
smee init
```
This will create a `.git-hooks` folder and (if appropriate) configure the local git repo to look for hooks there. If any hooks exist in that folder, git will be correctly configured to run them

If you're working for an organization that frequently starts new projects you may wish to keep a common repository of git hooks. For this you can pass the `--from-repo` command as such
```
smee init --from-repo https://<your git server>/<your git-hooks repo>
```
This will clone the specified repository into the `.git-hooks` as a submodule and then preform the correct configuration so that your scripts execute


### Adding a new hook
If you're configuring hooks in a git repo for the first time, smee can hel you setup a new hook
```
smee add <hook-name>
```
By default this will create a hook `<hook-name>.ps1` in your hooks folder. To override this pass the option `-t|--script-type`

