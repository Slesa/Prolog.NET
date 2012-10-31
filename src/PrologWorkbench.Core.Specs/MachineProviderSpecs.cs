using Machine.Fakes;
using Machine.Specifications;
using Prolog;
using Prolog.Code;
using PrologWorkbench.Core.Models;

namespace PrologWorkbench.Core.Specs
{
    [Subject(typeof(MachineProvider))]
    internal class When_initialy_using_machine_provider : MachineProviderSpecBase
    {
        Because of = () => _machine = Subject.Machine;
        It should_have_no_machine = () => _machine.ShouldBeNull();
        static PrologMachine _machine;
    }


    [Subject(typeof(MachineProvider))]
    internal class When_setting_machine : MachineProviderSpecBase
    {
        Establish context = () =>
                            {
                                _machine = CreateMachine();
                                Subject.MachineChanged += (s, e) =>
                                                          {
                                                              _handlerCalled = true;
                                                              _catchedMachine = e.Machine;
                                                          };
                            };
        Because of = () => Subject.Machine = _machine;
        It should_set_new_program = () => Subject.Machine.ShouldBeTheSameAs(_machine);
        It should_call_handler = () => _handlerCalled.ShouldBeTrue();
        It should_call_with_new_machine = () => _catchedMachine.ShouldBeTheSameAs(_machine);
        static PrologMachine _machine;
        static bool _handlerCalled;
        static PrologMachine _catchedMachine;
    }


    internal class MachineProviderSpecBase : WithSubject<MachineProvider>
    {
        protected static PrologMachine CreateMachine() 
        {
                    //var query = new Query(codeSentence);
            return PrologMachine.Create(An<Program>(), An<Query>()); //AppState.Program, query);
        }
        /*
        protected static void SetModified(Program program)
        {
            var t = program.GetType();
            //t.GetProperty("IsModified", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            t.InvokeMember("IsModified",
                           BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.SetProperty |
                           BindingFlags.Instance, null, program, new object[] { true });

        }
        protected const string SourceFilename = @"Resources\test.prolog";
         * */
    }
}