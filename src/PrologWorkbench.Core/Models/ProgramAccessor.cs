using System.IO;
using Prolog;
using PrologWorkbench.Core.Contracts;
using log4net;

namespace PrologWorkbench.Core.Models
{
    public class ProgramAccessor : ILoadOrSaveProgram
    {
        readonly ILog _logger = LogManager.GetLogger(typeof(ProgramProvider));

        public Program Load(string fileName)
        {
            Program program;
            try
            {
                program = Program.Load(fileName);
            }
            catch (FileNotFoundException ex)
            {
                _logger.Error("Coult not load file: ", ex);
                //CommonExceptionHandlers.HandleException(this, ex);
                return null;
            }
            catch (DirectoryNotFoundException ex)
            {
                _logger.Error("Coult not load file: ", ex);
                //CommonExceptionHandlers.HandleException(this, ex);
                return null;
            }
            catch (IOException ex)
            {
                _logger.Error("Coult not load file: ", ex);
                //CommonExceptionHandlers.HandleException(this, ex);
                return null;
            }
            return program;
        }

        public bool Save(string fileName, Program program)
        {
            if (program == null || string.IsNullOrEmpty(fileName))
            {
                return false;
            }

            if (!program.IsModified)
            {
                return true;
            }

            try
            {
                if (!fileName.Equals(program.FileName))
                    program.SaveAs(fileName);
                else
                    program.Save();
            }
            catch (FileNotFoundException ex)
            {
                _logger.Error("Coult not save file: ", ex);
                return false;
            }
            catch (DirectoryNotFoundException ex)
            {
                _logger.Error("Coult not save file: ", ex);
                return false;
            }
            catch (IOException ex)
            {
                _logger.Error("Coult not save file: ", ex);
                return false;
            }
            return true;
        }
    }
}