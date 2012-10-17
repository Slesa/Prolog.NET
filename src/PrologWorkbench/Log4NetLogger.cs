using Microsoft.Practices.Prism.Logging;
using log4net;

namespace Prolog.Workbench
{
    class Log4NetLogger : ILoggerFacade
    {
        readonly ILog _logger = LogManager.GetLogger(typeof (Log4NetLogger));
        public void Log(string message, Category category, Priority priority)
        {
            switch(category)
            {
                case Category.Debug:
                    _logger.Debug(message);
                    break;
                case Category.Info:
                    _logger.Info(message);
                    break;
                case Category.Warn:
                    _logger.Warn(message);
                    break;
                case Category.Exception:
                    _logger.Error(message);
                    break;
            }
        }
    }
}