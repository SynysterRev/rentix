import { TestBed } from '@angular/core/testing';

import { Lease } from './lease';

describe('Lease', () => {
  let service: Lease;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(Lease);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
