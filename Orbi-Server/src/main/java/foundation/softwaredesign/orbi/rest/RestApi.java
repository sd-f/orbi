package foundation.softwaredesign.orbi.rest;

import foundation.softwaredesign.orbi.model.virtual.GameObject;
import foundation.softwaredesign.orbi.model.virtual.Position;
import foundation.softwaredesign.orbi.model.virtual.World;
import foundation.softwaredesign.orbi.persistence.repo.ElevationRepository;
import foundation.softwaredesign.orbi.persistence.repo.GameObjectRepository;
import foundation.softwaredesign.orbi.service.VirtualWorldAdapter;
import foundation.softwaredesign.orbi.service.WorldFactory;

import javax.enterprise.context.RequestScoped;
import javax.inject.Inject;
import javax.validation.constraints.NotNull;
import javax.ws.rs.*;
import javax.ws.rs.core.MediaType;
import java.util.List;

import static javax.ws.rs.core.MediaType.APPLICATION_JSON;
import static javax.ws.rs.core.MediaType.APPLICATION_XML;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
@Path("/")
@Produces({APPLICATION_XML, APPLICATION_JSON})
@RequestScoped
public class RestApi {

    @Inject
    GameObjectRepository gameObjectRepository;

    @Inject
    ElevationRepository elevationRepository;

    @Inject
    VirtualWorldAdapter virtualWorldAdapter;
    @Inject
    VirtualWorldAdapter realWorldAdapter;

    @Inject
    WorldFactory worldFactory;

    @GET
    @Path("/init")
    @Produces(MediaType.TEXT_PLAIN)
    public String init() {
        worldFactory.init();
        return "OK";
    }

    @GET
    @Path("/elevation")
    public Position elevation(@NotNull(message = "latitude required") @QueryParam("latitude") Double latitude,
                              @NotNull(message = "longitude required") @QueryParam("longitude") Double longitude) {
        Position position = getPosition(latitude, longitude);
        position.setY(elevationRepository.getElevation(latitude, longitude));
        return position;
    }

    @GET
    @Path("/terrain")
    public World terrain(@NotNull(message = "latitude required") @QueryParam("latitude") Double latitude,
                         @NotNull(message = "longitude required") @QueryParam("longitude") Double longitude) {
        World terrainWorld = worldFactory.generateRasterAroundPosition(getPosition(latitude, longitude), 129); // todo resolution after db performance tuning
        worldFactory.addElevations(terrainWorld, getPosition(latitude, longitude));
        //virtualWorldAdapter.translate(terrainWorld, getPosition(latitude, longitude));
        //virtualWorldAdapter.scale(terrainWorld);
        System.out.println("terrain");
        return terrainWorld;
    }

    @GET
    @Path("/world")
    public World world(@NotNull(message = "latitude required") @QueryParam("latitude") Double latitude,
                       @NotNull(message = "longitude required") @QueryParam("longitude") Double longitude) {
        System.out.println("requesting world");
        return getWorld(latitude, longitude);
    }

    @POST
    @Path("/create")
    @Consumes({APPLICATION_XML, APPLICATION_JSON})
    public World create(@NotNull(message = "latitude required") @QueryParam("latitude") Double latitude,
                        @NotNull(message = "longitude required") @QueryParam("longitude") Double longitude,
                        World world) {
        System.out.println("creating cubes");
        realWorldAdapter.translate(world, getPosition(latitude, longitude));
        realWorldAdapter.scale(world);
        for (GameObject gameObject : world.getGameObjects()) {
            gameObjectRepository.save(gameObject);
            System.out.println("Saved gameObject");
        }
        return getWorld(latitude, longitude);
    }

    private World getWorld(Double latitude, Double longitude) {
        World world = new World();
        List<GameObject> gameObjectList = gameObjectRepository.findGameObjectsAround(latitude, longitude);
        world.setGameObjects(gameObjectList);
        // TODO elevation
        Position position = getPosition(latitude, longitude);
        //virtualWorldAdapter.scalePosition(position);
        virtualWorldAdapter.translate(world, position);
        virtualWorldAdapter.scale(world);

        return world;
    }

    private Position getPosition(Double latitude, Double longitude) {
        return new Position(longitude, 0.0, latitude);
    }

}
