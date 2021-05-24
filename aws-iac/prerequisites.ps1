# 0. Install package manager
#    Chocolatey is an open-source package manger for Windows
#    See https://chocolatey.org/
Set-ExecutionPolicy Bypass `
    -Scope Process `
    -Force;

[System.Net.ServicePointManager]::SecurityProtocol = [System.Net.ServicePointManager]::SecurityProtocol -bor 3072;
Invoke-Expression ((New-Object System.Net.WebClient).DownloadString('https://chocolatey.org/install.ps1'))

choco install terraform --version 0.15.3

choco install terraformer --version 0.8.13

#choco uninstall terraform
#choco uninstall terraformer

#terraform init
#terraformer import aws --resources="*" --filter="Name=tags.iac-tst" --regions="us-east-1"