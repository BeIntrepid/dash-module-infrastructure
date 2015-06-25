<Query Kind="Program" />

void Main()
{
	var infrastructureDir = new DirectoryInfo(Environment.CurrentDirectory);
	
	var fileBlacklist = new []{".gitignore","package.json","UpdateModules.linq","config.js"};
	var folderBlacklist = new []{".git","jspm_packages"};
	
	var sourceModulesDir = infrastructureDir.Parent;
	var infrastructureFiles = infrastructureDir.EnumerateFiles("*",SearchOption.AllDirectories)
									  .Where(f => !folderBlacklist.Any(f.DirectoryName.Contains))
									  .Where(f => !fileBlacklist.Any(f.Name.Contains));
	
	foreach (var srcModule in sourceModulesDir.EnumerateDirectories().Where(d=> d.FullName != infrastructureDir.FullName))
	{
		foreach (var file in infrastructureFiles)
		{
			file.Dump();
			
			var relativePath = file.FullName.Replace(infrastructureDir.FullName,"");
			var destinationName = (srcModule.FullName+relativePath);
			
			if(file.Name == "args.js" && File.Exists(destinationName))
			{
				continue;
			}
			var destinationFi = new FileInfo(destinationName);
			destinationFi.Directory.Create();
			File.Copy(file.FullName,destinationName,true);
			
			("Copied \r\n" + file.FullName + " to \r\n" + destinationName).Dump();
		}
	}
	
	
}

// Define other methods and classes here