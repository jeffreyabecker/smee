remote-item .\packed.zip
7z a .\packed.zip -r .\Smee\bin\Release\net5.0\*
choco pack .\chocolatey-package\smee.nuspec
