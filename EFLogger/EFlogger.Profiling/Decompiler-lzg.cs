using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using ICSharpCode.Decompiler;
using ICSharpCode.Decompiler.CSharp;
using Mono.Cecil;

namespace EFlogger.Profiling
{
    /// <summary>
    /// 原反编译在net471有Bug,采用新版本的反编译方式
    /// </summary>
    public static class Decompiler
    {
        public static string GetSourceCode(string pathToAssembly, string className, MethodBase methodBase)
        {
            try
            {
                if (methodBase.DeclaringType.Assembly != null && methodBase.DeclaringType.Assembly.IsDynamic)
                {
                    return methodBase.DeclaringType.FullName + " : " + methodBase.Name;
                }
                var methodDefinition = GetMethodDefinition(pathToAssembly, className, methodBase);
                return DecompileMethod(pathToAssembly, methodDefinition);
            }
            catch (Exception exception)
            {               
                return $"Exception in decompling. \r\n Message:{exception.Message}, \r\n Inner Exception:{exception.InnerException}, \r\n StackTrace:{exception.StackTrace}";
            }            
        }
        private static MethodDefinition GetMethodDefinition(string pathToAssembly, string className, MethodBase methodBase)
        {
            var assemblyDefinition = AssemblyDefinition.ReadAssembly(pathToAssembly);
            TypeDefinition assembleDefenition = assemblyDefinition.MainModule.Types.First(type => type.Name == className);
            var methodDefinitions = assembleDefenition.Methods
                .Where(method => method.Name == methodBase.Name && method.Parameters.Count == methodBase.GetParameters().Count());
            bool isFind = false;
            MethodDefinition methodDefinition = methodDefinitions.FirstOrDefault();
            foreach (var method in methodDefinitions)
            {
                for (int i = 0; i < method.Parameters.Count; i++)
                {
                    if (method.Parameters[i].ParameterType.Name != methodBase.GetParameters()[i].ParameterType.Name)
                    {
                        isFind = false;
                        break;
                    }
                    isFind = true;
                }
                if (isFind)
                {
                    methodDefinition = method;
                    break;
                }
            }
            return methodDefinition;

        }
        private static string DecompileMethod(string assemblyFileName,  MethodDefinition methodDefinition)
        {
            var decompiler = new CSharpDecompiler(assemblyFileName, new DecompilerSettings() { ThrowOnAssemblyResolveErrors = false });            
            string result = decompiler.DecompileAsString(methodDefinition);
            return result;
        }

    }
}
