# BizTalk MapperExtensions Functoid Wizard
BizTalk MapperExtensions Functoid Wizard is a Custom Functoid Project Wizard for Visual Studio 2015. It allows you to create a new Functoid project for BizTalk Server 2016 without having to create manually the project, in other words, having to manually create:
-	A new class library project in Visual Studio 2015;
-	Add a reference to the Microsoft.BizTalk.BaseFunctoids assembly;
-	Add New Class and having to code the entire class;

This Wizard will do this process automatically, and used in conjunction with BizTalk Server: Custom BizTalk Functoid item template for Visual Studio 2015 will facilitate and significantly expedite the development process of our projects. The only thing you will need to do is create a new Custom Functoid Project and a wizard will pop-up, fill in the required fields and when you finish the process a new Custom Functoid project is built. Just change the execution method with your own algorithms, build your project and it’s done. You’ll see your new Functoid in BizTalk Server 2015 Mapper once you import it to Visual Studio Toolbox.

## Installation
To use this project template just unzip the Downloaded folder and run the setup file available on “Installation Files” folder.
Once you run the setup file follow these steps:
-	On the “Welcome to the BizTalk MapperExtensions Functoid Template Setup Wizard” screen, click “Next”.
-	On the “Select Installation Folder” screen, select the folder where you want to install the Wizard and select the option “Everyone”. Click “Next” to continue.
-	On the “Confirm Installation” screen, confirm your intent to install by clicking “Next”
-	On the “Installation Complete” screen, click close and the installation is complete

Now you can see a new BizTalk project template option in your Visual Studio 2015 under “BizTalk projects”.

## Create New Functoid Project
To create a new functoid using this project template choose the option “BizTalk Server Functoid Project” in Visual Studio 2015 and a Wizard will pop-up. Follow the requirements of this wizard to create the Functoid.##
-	In the Start screen click “Next”
-	In the “General Project Properties” screen, fill the Functoid class name, namespace and create or select a new Strong Key (you can use an existing one) . Click “Next” to continue.
-	In the “Functoid Properties” screen, define the Functoid ID (must be greater than 10000), the functoid name, tooltip (short description) and description (full description) . Click “Next” to continue.
-	In the second “Functoid Properties” screen, choose the functoid category and the implementation language (the language in wich you want to implement your functoid behavior code): C# or VB.NET. Click “Next” to continue.
-	In the “Functoid Parameters and Connection Types” screen, define the functoid function name, function inputs and types and output and types. Click “Next” to continue.
-	On the “Result” screen, click “Finish” to create the Visual Studio project.

Visual Studio will create a new Project based on your implementation language and definitions provided in the Wizard.
-	Open the generated class and implement your code

Also special thanks to Rui Silva (https://www.linkedin.com/in/ruisilva450/) who helped me finish this project.
