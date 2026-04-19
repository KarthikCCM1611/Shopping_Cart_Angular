import { TestBed } from '@angular/core/testing';

import { ShoppingMasterService } from './shopping-master.service';

describe('ShoppingMasterService', () => {
  let service: ShoppingMasterService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ShoppingMasterService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
