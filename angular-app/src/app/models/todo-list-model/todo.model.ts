export interface TodoItem {
  id: string;
  title: string;
  isDone: boolean;
}

export const TODO_API = {
  GET_ALL: 'GetToDoLists',
  ADD: 'AddNewToDo',
  UPDATE: 'UpdateToDo',
  DELETE: 'DeleteToDo'
};