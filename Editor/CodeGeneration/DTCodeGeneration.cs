using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Compilation;

namespace DrawerTools.CodeGeneration
{
    public static class DTCodeGeneration
    {
        private static Action _completeCompilationAction;

        public static void CreateOrUpdateCS(string path, IBuilder builder, Action complete = null)
        {
            var code = builder.Build();
            CreateOrUpdateCS(path, code, complete);
        }

        public static void CreateOrUpdateCS(string path, string code, Action complete = null)
        {
            _completeCompilationAction = complete;
            using (var sw = (File.Exists(path)) ? new StreamWriter(path, false) : File.CreateText(path))
            {
                sw.Write(code);
            }

            CompilationPipeline.RequestScriptCompilation();
            CompilationPipeline.compilationFinished -= AtCompilationFinished;
            CompilationPipeline.compilationFinished += AtCompilationFinished;
        }

        private static void AtCompilationFinished(object obj)
        {
            _completeCompilationAction?.Invoke();
            _completeCompilationAction = null;
        }
    }
}