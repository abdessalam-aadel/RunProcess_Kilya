using System;
using System.Diagnostics;
using System.IO;

namespace RunProcess_Kilya
{
    public class RunProcess
    {
        // Run ffmpeg process
        public string RunFFmpeg(string _argument, bool _wait)
        {
            string ffmpeg;
            // Start Condition : if you have win32 or win64
            if (Environment.Is64BitOperatingSystem)
                ffmpeg = @"tools\x64\bin\ffmpeg.exe"; // path of FFmpeg tools for win32
            else
                ffmpeg = @"tools\x32\bin\ffmpeg.exe"; // path of FFmpeg tools for win64
            // Create a process info
            ProcessStartInfo pInfo = new ProcessStartInfo(ffmpeg, _argument)
            {
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };
            // Create the output and streamreader to get the output
            string _output = null; StreamReader srOutput = null;
            // try the process
            try
            {
                // Run the process
                Process p = Process.Start(pInfo);
                if (_wait)
                {
                    p.BeginOutputReadLine();
                    p.BeginErrorReadLine();
                }
                else
                {
                    // get the output
                    srOutput = p.StandardError;
                    // put it in a string
                    _output = srOutput.ReadToEnd();
                }
                // wait for exit and close the process ..
                p.WaitForExit();
                p.Close();
            }
            catch (Exception ex)
            {
                _output = ex.Message;
            }
            finally
            {
                //if we succeded, Dispose the streamreader
                srOutput?.Dispose();
            }
            return _output;
        }

        // Run cmd process
        public string RunCmd(string _argument, bool _ff)
        {
            string ffmpeg = string.Empty;
            if(_ff)
            {
                // Start Condition : if you have win32 or win64
                if (Environment.Is64BitOperatingSystem)
                    ffmpeg = @"tools\x64\bin\ffmpeg.exe"; // path of FFmpeg tools for win32
                else
                    ffmpeg = @"tools\x32\bin\ffmpeg.exe"; // path of FFmpeg tools for win64
            }
            //create a process info
            ProcessStartInfo oInfo = new ProcessStartInfo("cmd.exe")
            {
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardInput = true
            };
            //Create the output and streamreader to get the output
            string output = null; StreamReader srOutput = null;
            // try the process
            try
            {
                // Run the process
                Process p = Process.Start(oInfo);
                p.StandardInput.WriteLine(ffmpeg + _argument);
                // Flush & Close StandarInput	
                p.StandardInput.Flush();
                p.StandardInput.Close();
                // get the output
                srOutput = p.StandardOutput;
                // put it in a string
                output = srOutput.ReadToEnd();
                // wait for exit and close the process ..
                p.WaitForExit();
                p.Close();
            }
            catch (Exception ex)
            {
                output = ex.Message;
            }
            finally
            {
                //if we succeded, Dispose the streamreader
                srOutput?.Dispose();
            }
            return output;
        }
    }
}
