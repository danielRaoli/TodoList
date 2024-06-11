using Microsoft.AspNetCore.Components;
using MudBlazor;
using TodoList.Core.Enums;
using TodoList.Core.Repositories;
using TodoList.Core.Requests.Todo;


namespace Todo.App.Pages.TodoPages
{
    public class GetAllPageComponent : ComponentBase
    {
        public bool IsBusy { get; set; } = false;
        public List<TodoList.Core.Entities.Todo?> Todos { get; set; } = [];
        public CreateRequest Model { get; set; } = new();


        [Inject]
        public ITodoRepository Handler { get; set; } = null!;
        [Inject]
        public ISnackbar Snackbar { get; set; } = null!;
        [Inject]
        public IDialogService Dialog { get; set; }


        protected async override Task OnInitializedAsync()
        {
            IsBusy = true;
            try
            {
                var request = new GetAllRequest();
                var result = await Handler.GetAllTodos(request);

                if (result.IsSuccess)
                {
                    Todos = result.Data;
                }
                else
                {
                    Snackbar.Add(result.Message, Severity.Error);
                }
            }
            catch (Exception ex)
            {
                Snackbar.Add(ex.Message, Severity.Error);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task OnDeleButtonClickAsync(int id, string name)
        {

            var result = await Dialog.ShowMessageBox("Excuir Tarefa", $"Deseja Excluir a tarefa {name}", yesText: "Sim", cancelText: "Não");
            if (result is true)
            {
                await OnDeleteAsync(id);
                StateHasChanged();
            }

        }

        public async Task OnDeleteAsync(int id)
        {
            try
            {
                var request = new DeleteRequest { Id = id };
                var result = await Handler.DeleteTodo(request);
                if (result.IsSuccess)
                {

                    Todos.RemoveAll(x => x.Id == id);

                    Snackbar.Add(result.Message, Severity.Info);
                }
                else
                {
                    Snackbar.Add(result.Message, Severity.Error);
                }
            }
            catch (Exception ex)
            {
                Snackbar.Add(ex.Message, Severity.Error);
            }
        }

        public async Task OnValidSubmitAsync()
        {
            IsBusy = true;
            try
            {
                var result = await Handler.CreateTodo(this.Model);
                if (result.IsSuccess)
                {
                    Snackbar.Add(result.Message, Severity.Success);
                    Todos.Add(result.Data);
                    StateHasChanged();
                }
                else
                {
                    Snackbar.Add(result.Message, Severity.Error);
                }

            }
            catch (Exception ex)
            {
                Snackbar.Add(ex.Message, Severity.Error);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public bool Converter(TodoStatus status)
        {
            return status == TodoStatus.Complete;
        }


        public async Task OnChangedAsync( object isComplete, int id)
        {
            IsBusy = true;
          
            try
            {
               
                var request = new CompletTodoRequest { Id = id };
                var result = await Handler.AlternateStatus(request);
                var todo = Todos.FirstOrDefault(x => x.Id == id);

                if (isComplete is bool isCompleteBool)
                {
                    todo.IsComplete = isCompleteBool ? TodoStatus.Incomplete : TodoStatus.Complete;
                    Snackbar.Add(result.Message, isCompleteBool ? Severity.Error : Severity.Success);
                }

                StateHasChanged();

            }
            catch (Exception ex)
            {
                Snackbar.Add(ex.Message, Severity.Error);
            }
            finally
            {
                IsBusy = false;
            }

        }

        public string LabelAlternateButton(TodoStatus status)
        {
            var isComplete = Converter(status);
            return isComplete ? "Desfazer" : "Completar";
        }
    }
}
