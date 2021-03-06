﻿using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using TokenManage.Exceptions;

namespace TokenManage.Domain
{
    public class TMProcess
    {
        public string ProcessName { get; }
        public int ProcessId { get; }

        public string NamedPipeInputPath { get; }
        public string NamedPipeOutputPath { get; }
        public string NamedPipeErrorPath { get; }

        public TMProcess(Process process)
        {
            this.ProcessName = process.ProcessName;
            this.ProcessId = process.Id;
            this.NamedPipeErrorPath = null;
            this.NamedPipeInputPath = null;
            this.NamedPipeOutputPath = null;
        }

        private TMProcess(string processName, int pid, string stdinPath = null, string stdoutPath = null, string stderrPath = null)
        {
            this.ProcessName = processName;
            this.ProcessId = pid;
            this.NamedPipeErrorPath = stderrPath;
            this.NamedPipeInputPath = stdinPath;
            this.NamedPipeOutputPath = stdoutPath;
        }

        public static List<TMProcess> GetProcessByName(string name)
        {
            Process[] processes = Process.GetProcessesByName(name);
            return processes.Select(x => new TMProcess(x)).ToList();
        }

        public static TMProcess GetProcessById(int pid)
        {
            Process p = Process.GetProcessById(pid);
            if (p == null)
                throw new ProcessNotFoundException();

            return new TMProcess(p);
        }

        public static List<TMProcess> GetAllProcesses()
        {
            List<Process> processes = new List<Process>(Process.GetProcesses());
            return processes.Select(x => new TMProcess(x)).ToList();
        }

        public static TMProcess FromValues(string processName, int pid, string stdinPath = null, string stdoutPath = null, string stderrPath = null)
        {
            return new TMProcess(processName, pid, stdinPath, stdoutPath, stderrPath);
        }
    }
}
