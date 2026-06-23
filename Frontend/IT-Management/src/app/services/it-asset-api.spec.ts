import { TestBed } from '@angular/core/testing';

import { ITAssetApi } from './it-asset-api';

describe('ITAssetApi', () => {
  let service: ITAssetApi;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ITAssetApi);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
