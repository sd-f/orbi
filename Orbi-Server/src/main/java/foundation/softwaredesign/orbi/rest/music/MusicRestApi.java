package foundation.softwaredesign.orbi.rest.music;

import foundation.softwaredesign.orbi.model.music.Song;
import foundation.softwaredesign.orbi.service.music.MusicService;

import javax.enterprise.context.RequestScoped;
import javax.inject.Inject;
import javax.ws.rs.Consumes;
import javax.ws.rs.GET;
import javax.ws.rs.Path;
import javax.ws.rs.Produces;

import static javax.ws.rs.core.MediaType.APPLICATION_JSON;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
@Path("/music")
@Produces({APPLICATION_JSON})
@Consumes({APPLICATION_JSON})
@RequestScoped
public class MusicRestApi {

    @Inject
    MusicService musicService;

    @GET
    @Path("/next")
    public Song statistics() {
        return musicService.getNextRandomSong();
    }

}
