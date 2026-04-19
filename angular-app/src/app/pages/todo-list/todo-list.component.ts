import { Component, OnInit } from '@angular/core';
import { TodoItem } from '../../models/todo-list-model/todo.model';
import { FormsModule } from '@angular/forms';
import { NgClass, NgFor, NgIf } from '@angular/common';
import { TodoService } from '../../services/todo/todo.service';

@Component({
  selector: 'app-todo-list',
  standalone: true,
  imports: [FormsModule, NgIf, NgFor, NgClass],
  templateUrl: './todo-list.component.html',
  styleUrl: './todo-list.component.css'
})
export class TodoListComponent implements OnInit {
  todos: TodoItem[] = [];
  newTitle = '';

  constructor(private api: TodoService) { }

  ngOnInit() {
    this.load();
  }
  load() {
    // this.api.getAll().subscribe(t => this.todos = t);
    this.api.getAll().subscribe({
      next: (result: TodoItem[]) => {
        this.todos = result;
      },
      error: (err) => {
        alert(err.message); // Or handle UI update
      }
    });
  }

  add() {
    if (!this.newTitle.trim()) return;
    // this.api.add(this.newTitle).subscribe(() => {
    //   this.newTitle = '';
    //   this.load();
    // });
    this.api.add(this.newTitle).subscribe({
      next: (result: TodoItem) => {
        this.newTitle = '';
        this.load();
      },
      error: (err) => {
        alert(err.message); // Or handle UI update
      }
    });
  }

  toggle(t: TodoItem) {
    // this.api.update({ ...t, isDone: !t.isDone }).subscribe(() => this.load());
    this.api.update({ ...t, isDone: !t.isDone }).subscribe({
      next: (result) => {
        this.load()
      },
      error: (err) => {
        alert(err.message); // Or handle UI update
      }
    });
  }

  remove(id: string) {
    this.api.delete(id).subscribe(() => this.load());
  }
}
