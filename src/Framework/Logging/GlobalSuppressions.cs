
// This file is used by Code Analysis to maintain SuppressMessage 
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given 
// a specific target and scoped to a namespace, type, member, etc.

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "CA1716:Rename virtual/interface member ILogger.Error(object) so that it no longer conflicts with the reserved language keyword 'Error'. Using a reserved keyword as the name of a virtual/interface member makes it harder for consumers in other languages to override/implement the member.", Justification = "OK here.", Scope = "member", Target = "~M:Logging.ILogger.Error(System.Object)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "CA1716:Rename virtual/interface member ILogger.Error(object, Exception) so that it no longer conflicts with the reserved language keyword 'Error'. Using a reserved keyword as the name of a virtual/interface member makes it harder for consumers in other languages to override/implement the member.", Justification = "OK here.", Scope = "member", Target = "~M:Logging.ILogger.Error(System.Object,System.Exception)")]