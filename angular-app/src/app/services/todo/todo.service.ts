import { inject, Injectable } from '@angular/core';
import { catchError, Observable, throwError } from 'rxjs';
import { TODO_API, TodoItem } from '../../models/todo-list-model/todo.model';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class TodoService {

  // private base = 'https://localhost:7168/api/todo';
  private base = 'https://localhost:7168/api/todoclass';

  private http = inject(HttpClient);
  // constructor(private http: HttpClient) { }

  getAll(): Observable<TodoItem[]> {
    // return this.http.get<TodoItem[]>(this.base);
    return this.http.get<TodoItem[]>(`${this.base}/${TODO_API.GET_ALL}`).pipe(
      catchError(this.handleError)
    );
  }
  add(title: string): Observable<TodoItem> {
    // return this.http.post<TodoItem>(this.base, { title });
    // return this.http.post<TodoItem>(this.base, { title }, {
    //   headers: {
    //     'Content-Type': 'application/json'
    //   }
    // });
    // return this.http.post<TodoItem>(`${this.base}/AddNewToDo`, { title }).pipe(
    //   catchError(this.handleError)
    // );
    return this.http.post<TodoItem>(`${this.base}/${TODO_API.ADD}`, { title }).pipe(
      catchError(this.handleError)
    );
  }
  update(item: TodoItem): Observable<void> {
    // return this.http.put<void>(`${this.base}/UpdateToDo/${item.id}`, item);
    return this.http.put<void>(`${this.base}/${TODO_API.UPDATE}/${item.id}`, item).pipe(
      catchError(this.handleError)
    );
  }
  delete(id: string): Observable<void> {
    // return this.http.delete<void>(`${this.base}/DeleteToDo/${id}`);
    return this.http.delete<void>(`${this.base}/${TODO_API.DELETE}/${id}`).pipe(
      catchError(this.handleError)
    );
  }

  private handleError(error: HttpErrorResponse) {
    const errorDetails: string = (error.error && !(error.error instanceof ProgressEvent)) ? error.error : 'Something went wrong. Please try again later.'
    if (error.error instanceof ErrorEvent) {
      // Client-side or network error
      console.error('Client-side error:', error.error.message);
    } else {
      // Backend error
      console.error(
        `Server returned code ${error.status}, ` +
        `Message: ${errorDetails}`
      );
    }
    // Return a user-facing error message
    return throwError(() => new Error(errorDetails));
  }
}
