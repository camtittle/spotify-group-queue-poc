import { AfterViewInit, ChangeDetectorRef, Component, ElementRef, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { AuthenticationService } from '../../../services/index';
import { from, fromEvent, Observable } from 'rxjs';
import { distinctUntilChanged, filter, map, switchMap, tap, throttleTime } from 'rxjs/operators';
import { AccessToken, QueueTrack, TrackSearchResult, SpotifyTrack } from '../../../models/index';
import { PartyHubService } from '../../../services/party-hub.service';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.scss']
})
export class SearchComponent implements OnInit, OnDestroy, AfterViewInit {

  @ViewChild('inputContainer',  {read: ElementRef}) inputContainer: ElementRef;
  @ViewChild('searchInput') searchInput: ElementRef;

  public searchInputValue: string;
  public results: SpotifyTrack[];
  private currentUser: AccessToken;
  private typeahead: Observable<SpotifyTrack[]>;
  public containerPaddingTop: number;


  constructor(private authService: AuthenticationService,
              private partyHubService: PartyHubService,
              private cdRef: ChangeDetectorRef) {
  }

  async ngOnInit() {
    // Typeahead behavior
    this.typeahead = fromEvent<any>(this.searchInput.nativeElement, 'input').pipe(
      throttleTime(1000, undefined, {leading: true, trailing: true}),
      map(event => event.target.value),
      filter(query => query.length > 0),
      tap(query => console.log(query)),
      distinctUntilChanged(),
      switchMap(query => this.searchSpotify(query))
    );

    this.typeahead.subscribe(result => {
      this.results = result;
    });

    // Access token subscription
    this.authService.currentUser$.subscribe(user => {
      this.currentUser = user;
    });

  }

  ngOnDestroy() {

  }

  ngAfterViewInit() {
    this.containerPaddingTop = this.inputContainer.nativeElement.clientHeight;
    this.searchInput.nativeElement.focus();
    this.cdRef.detectChanges();
  }

  private searchSpotify(query: string): Observable<SpotifyTrack[]> {
    return from<TrackSearchResult>(this.partyHubService.invoke('searchSpotifyTracks', query)).pipe(
      map(response => response.tracks.items)
    );
  }

  public async onTrackClick(track: SpotifyTrack) {
    if (!this.currentUser || !this.currentUser.username) {
      console.error('Cannot add track to queue - no current user');
      return;
    }

    const queueTrack = <QueueTrack> {
      spotifyUri: track.uri,
      title: track.name,
      artist: track.artists.map(x => x.name).join(', '),
      durationMillis: track.duration_ms
    };

    await this.partyHubService.invoke('addTrackToQueue', queueTrack);
  }

}
