/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { ProviderService } from './Provider.service';

describe('Service: Provider', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [ProviderService]
    });
  });

  it('should ...', inject([ProviderService], (service: ProviderService) => {
    expect(service).toBeTruthy();
  }));
});
